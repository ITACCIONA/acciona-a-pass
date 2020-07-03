module.exports = {
  testURL: 'http://localhost',
  preset: 'jest-preset-angular',
  setupFilesAfterEnv: ['<rootDir>/src/setupJest.ts'],
  coveragePathIgnorePatterns: [
    '/node_modules/',
    '/src/jestGlobalMocks.ts',
    '/src/setupJest.ts',
    '/src/environments'
  ],
  coverageThreshold: {
    global: {
      branches: 80,
      functions: 80,
      lines: 80,
      statements: -10
    }
  },
  globals: {
    ENV: 'testing'
  },
  moduleNameMapper: {
    '^@commons': '<rootDir>/src/app/commons/index'
  }
};
