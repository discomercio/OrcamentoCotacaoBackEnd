export const environment = {
  autenticaStorageSession: true, //se false, gaurda no localstorage (o usuário pode escolher na tela)
  minutosRenovarTokenAntesExpirar: 60*24*4, //token emitido com 7 dias, renovamos 4 dais antes de vencer

  esperaAvisos: 3000,
  esperaErros: 5000,
  apiUrl: '/api/',
  production: true
};
