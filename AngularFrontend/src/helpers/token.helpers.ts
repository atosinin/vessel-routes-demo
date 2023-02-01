import jwt_decode from "jwt-decode";

export const TokenHelpers = {
  isValid
}

function isValid(token: string): boolean {
  const decodedToken: any = jwt_decode(token);
  const inOneMinute = 60 + Date.now() / 1000;
  if (typeof decodedToken.exp !== 'undefined' && decodedToken.exp < inOneMinute) {
    return false;
  }
  return true;
}
