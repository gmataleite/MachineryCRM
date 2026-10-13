import { describe, it, expect } from 'vitest';
import { normalizeTaxId, detectTaxIdType, validateTaxId } from './taxId';

describe('TaxId Utils - Padrão Strategy', () => {
  it('1. normalizeTaxId remove ".", "-" e "/"', () => {
    expect(normalizeTaxId('12.345.678/0001-90')).toBe('12345678000190');
    expect(normalizeTaxId('111.222.333-44')).toBe('11122233344');
  });

  it('2. detectTaxIdType detecta CPF para BR', () => {
    expect(detectTaxIdType('BR', '11122233344')).toBe('CPF');
  });

  it('3. detectTaxIdType detecta CNPJ para BR', () => {
    expect(detectTaxIdType('BR', '12345678000190')).toBe('CNPJ');
    expect(detectTaxIdType('BR', '12ABC678000190')).toBe('CNPJ');
  });

  it('4. detectTaxIdType retorna null para comprimento inválido ou país sem validador', () => {
    expect(detectTaxIdType('BR', '123')).toBeNull();
    expect(detectTaxIdType('US', '12345678000190')).toBeNull();
  });

  it('5. CPF válido sem máscara retorna true', () => {
    expect(validateTaxId('BR', '52998224725')).toBe(true);
  });

  it('6. CPF válido com máscara retorna true', () => {
    expect(validateTaxId('BR', '529.982.247-25')).toBe(true);
  });

  it('7. CNPJ válido sem máscara retorna true', () => {
    expect(validateTaxId('BR', '11222333000181')).toBe(true);
  });

  it('8. CNPJ válido com máscara retorna true', () => {
    expect(validateTaxId('BR', '11.222.333/0001-81')).toBe(true);
  });

  it('9. CNPJ alfanumérico válido retorna true', () => {
    expect(validateTaxId('BR', '12ABC34501DE35')).toBe(true);
  });

  it('10. CPF com dígito verificador inválido retorna false', () => {
    expect(validateTaxId('BR', '52998224726')).toBe(false);
  });

  it('11. CNPJ com dígito verificador inválido retorna false', () => {
    expect(validateTaxId('BR', '11222333000182')).toBe(false);
  });

  it('12. Valores repetidos retornam false', () => {
    expect(validateTaxId('BR', '11111111111')).toBe(false);
    expect(validateTaxId('BR', '22222222222222')).toBe(false);
  });

  it('13. Caracteres proibidos retornam false', () => {
    expect(validateTaxId('BR', '11.222.333/0001-*1')).toBe(false);
  });

  it('14. CPF com letras retorna false', () => {
    expect(validateTaxId('BR', '111222333AA')).toBe(false);
  });

  it('15. CNPJ com dígitos verificadores alfabéticos retorna false', () => {
    expect(validateTaxId('BR', '112223330001AA')).toBe(false);
  });

  it('16. TaxId vazio é tratado como válido e permitido enviar nulo no payload', () => {
    expect(validateTaxId('BR', '')).toBe(true);
    expect(validateTaxId('BR', null as never)).toBe(true);
  });

  it('17. País desconhecido mantém compatibilidade com backend retornando true', () => {
    expect(validateTaxId('US', 'QUALQUER-DOCUMENTO-INVALIDO')).toBe(true);
  });
});
