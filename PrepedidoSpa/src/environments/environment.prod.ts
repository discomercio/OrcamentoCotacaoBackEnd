export const environment = {
  autenticaStorageSession: true, //se false, gaurda no localstorage (o usuário pode escolher na tela)
  minutosRenovarTokenAntesExpirar: 60*24*3, //token emitido com 7 dias, renovamos 3 dais antes de vencer

  esperaAvisos: 3000,
  esperaErros: 5000,
  apiUrl: 'http://its-appdev02:2000/api/',
  versaoApi: 'DEBUG',
  production: true
};
