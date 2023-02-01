export interface AuditableDTO {
  createdBy: string;
  createdOn: Date;
  lastModifiedBy: string;
  lastModifiedOn: Date;
}

export interface WhateverDTO extends AuditableDTO
{
  whateverId: number;
  name: string;
  description: string;
  userAccountId: string;
}
