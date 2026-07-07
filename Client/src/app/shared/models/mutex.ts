export class Mutex {
  private lock$: Promise<void> = Promise.resolve();

  use<T>(fn: () => Promise<T>): Promise<T> {
    const result = this.lock$.then(() => fn());

    this.lock$ = result.then(
      () => {},
      () => {},
    );

    return result;
  }
}
