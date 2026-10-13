// Interface base para todos os validadores de país
export interface TaxIdValidator {
  detectType(value: string): string | null;
  validate(value: string): boolean;
}

export function normalizeTaxId(value: string): string {
  if (!value) return '';
  return value.replace(/[./-]/g, '');
}

// Implementação específica para o Brasil
const brTaxValidator: TaxIdValidator = {
  detectType(value: string): 'CPF' | 'CNPJ' | null {
    const clean = normalizeTaxId(value);
    if (clean.length === 11 && /^\d{11}$/.test(clean)) return 'CPF';
    if (clean.length === 14) return 'CNPJ';
    return null;
  },

  validate(value: string): boolean {
    const clean = normalizeTaxId(value);
    if (/[^a-zA-Z0-9]/.test(clean)) return false;
    if (/^([a-zA-Z0-9])\1+$/.test(clean)) return false;

    const type = this.detectType(value);
    if (type === 'CPF') return validateCPF(clean);
    if (type === 'CNPJ') return validateCNPJ(clean);

    return false;
  }
};

// Funções auxiliares privadas do Brasil
function validateCPF(cpf: string): boolean {
  if (/[a-zA-Z]/.test(cpf)) return false;

  let sum = 0,
    remainder;
  for (let i = 1; i <= 9; i++) sum += parseInt(cpf.substring(i - 1, i)) * (11 - i);
  remainder = (sum * 10) % 11;
  if (remainder === 10 || remainder === 11) remainder = 0;
  if (remainder !== parseInt(cpf.substring(9, 10))) return false;

  sum = 0;
  for (let i = 1; i <= 10; i++) sum += parseInt(cpf.substring(i - 1, i)) * (12 - i);
  remainder = (sum * 10) % 11;
  if (remainder === 10 || remainder === 11) remainder = 0;
  if (remainder !== parseInt(cpf.substring(10, 11))) return false;

  return true;
}

function validateCNPJ(cnpj: string): boolean {
  const getNumericValue = (char: string) => {
    const code = char.charCodeAt(0);
    if (code >= 48 && code <= 57) return code - 48;
    if (code >= 65 && code <= 90) return code - 48;
    if (code >= 97 && code <= 122) return code - 80;
    return NaN;
  };

  const weights1 = [5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2];
  const weights2 = [6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2];

  const verifyDigit1 = cnpj.charAt(12);
  const verifyDigit2 = cnpj.charAt(13);
  if (/[a-zA-Z]/.test(verifyDigit1) || /[a-zA-Z]/.test(verifyDigit2)) return false;

  let sum = 0;
  for (let i = 0; i < 12; i++) sum += getNumericValue(cnpj.charAt(i)) * weights1[i];
  let remainder = sum % 11;
  const digit1 = remainder < 2 ? 0 : 11 - remainder;
  if (parseInt(verifyDigit1) !== digit1) return false;

  sum = 0;
  for (let i = 0; i < 13; i++) sum += getNumericValue(cnpj.charAt(i)) * weights2[i];
  remainder = sum % 11;
  const digit2 = remainder < 2 ? 0 : 11 - remainder;
  if (parseInt(verifyDigit2) !== digit2) return false;

  return true;
}

// Registro de validadores (fácil expansão futura)
const validators: Record<string, TaxIdValidator> = {
  BR: brTaxValidator
};

function getValidator(countryCode: string): TaxIdValidator | undefined {
  if (!countryCode) return undefined;
  return validators[countryCode.toUpperCase()];
}

// APIs Expostas
export function detectTaxIdType(countryCode: string, value: string): string | null {
  const validator = getValidator(countryCode);
  return validator ? validator.detectType(value) : null;
}

export function validateTaxId(countryCode: string, value: string): boolean {
  if (!value || value.trim() === '') return true;

  const validator = getValidator(countryCode);
  // Se não existe validador para o país, assume como válido (mantém compatibilidade)
  if (!validator) return true;

  return validator.validate(value);
}
