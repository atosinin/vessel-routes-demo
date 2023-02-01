export enum AlertType {
  Error = 0,
  Warning = 1,
  Success = 2,
  Info = 3,
}

export class Alert {
  id: number = 0;
  type: AlertType = AlertType.Error;
  message: string = "";

  constructor(init?: Partial<Alert>) {
    Object.assign(this, init);
  }
}
