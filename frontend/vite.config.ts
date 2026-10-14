import { defineConfig } from 'vitest/config';
import react from '@vitejs/plugin-react';
import { VitePWA } from 'vite-plugin-pwa';

export default defineConfig({
  plugins: [react(), VitePWA()],
  test: {
    environment: 'jsdom',
    setupFiles: ['./src/test/setup.ts'],
    globals: false,
    include: ['**/*.test.ts', '**/*.test.tsx'],
    clearMocks: true,
    restoreMocks: true,
    reporters: ['dot']
  }
});
