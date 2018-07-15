// The file contents for the current environment will overwrite these during build.
// The build system defaults to the dev environment which uses `environment.ts`, but if you do
// `ng build --env=prod` then `environment.prod.ts` will be used instead.
// The list of which env maps to which file can be found in `.angular-cli.json`.

export const environment = {
  itemLimit: 10,
  production: false,
  apiUrl: 'https://rssewebdev.trg.com:666/data/',
  portfolioManager: 'sstre',
  portfolioManagerDistribution: 'WIT_TEST_Portfolio_Management',
  digitalStrategyDistribution: 'WIT_TEST_Digital_Strategy',
  adminModuleLink: 'https://rssewebdev.trg.com:666/admin',
  securePort: 4443
};
