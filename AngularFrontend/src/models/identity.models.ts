export interface RegisterModel {
  email: string;
  firstName: string;
  lastName: string;
  password: string;
  confirmPassword: string;
  hasAcceptedTerms: boolean;
}

export interface LoginModel {
  email: string;
  password: string;
}

export interface ForgottenPasswordModel {
  email: string;
}

export interface ChangePasswordModel {
  email: string;
  token: string;
  password: string;
  confirmPassword: string;
}

export interface UserRoleDTO {
  id: string;
  name: string;
  description: string;
}

export interface UserAccountDTO {
  id: string;
  email: string;
  firstName: string;
  lastName: string;
  picture: string | null;
  termsAcceptedOn: Date;
  roles: UserRoleDTO[];
}

export interface TokenModel {
  token: string;
}

export interface WhateverUser extends UserAccountDTO {
  token: TokenModel;
}
