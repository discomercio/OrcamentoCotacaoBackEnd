// This file can be replaced during build by using the `fileReplacements` array.
// `ng build --prod` replaces `environment.ts` with `environment.prod.ts`.
// The list of file replacements can be found in `angular.json`.

export const environment = {
  autenticaStorageSession: false, //se false, gaurda no localstorage (o usuário pode escolher na tela)
  minutosRenovarTokenAntesExpirar: 60*24*4, //token emitido com 7 dias, renovamos 4 dais antes de vencer

  esperaAvisos: 3000,
  esperaErros: 5000,
  
  //apiUrl: 'http://its-appdev:9000/api/', //no servidor, nao funciona pq estamos bloqueando o CORS
  // apiUrl: 'http://x.com.br/', //no servidor, nao funciona pq estamos bloqueando o CORS
  apiUrl: 'http://localhost:60877/api/',
  versaoApi: 'DEBUG',
  production: false
};

/*
 * For easier debugging in development mode, you can import the following file
 * to ignore zone related error stack frames such as `zone.run`, `zoneDelegate.invokeTask`.
 *
 * This import should be commented out in production mode because it will have a negative impact
 * on performance if an error is thrown.
 */
// import 'zone.js/dist/zone-error';  // Included with Angular CLI.
