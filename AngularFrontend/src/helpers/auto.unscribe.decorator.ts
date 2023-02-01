export function AutoUnsubscribeDecorator(doNotUnsubscribeSubscriptions: string[] = []) {
  return function (constructor: Function) {
    const originalNgOnDestroy = constructor.prototype.ngOnDestroy;
    constructor.prototype.ngOnDestroy = function () {
      for (let prop in this) {
        const property = this[prop];
        if (!doNotUnsubscribeSubscriptions.includes(prop)) {
          if (property && (typeof property.unsubscribe === "function")) {
            property.unsubscribe();
          }
        }
      }
      if (originalNgOnDestroy && typeof originalNgOnDestroy === 'function') {
        originalNgOnDestroy.apply(this, arguments);
      }
    };
  }
}
