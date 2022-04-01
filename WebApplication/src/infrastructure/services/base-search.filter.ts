export abstract class BaseSearchFilter {
  private defaultLimit: number;

  limit: number;
  offset: number;

  constructor(defaultLimit: number) {
    this.defaultLimit = defaultLimit;

    this.limit = defaultLimit;
    this.offset = 0;
  }

  clear(): void {
    Object.keys(this)
      .filter((key: string) => key !== 'defaultLimit')
      .forEach((key: string) => this[key] = null);

    this.limit = this.defaultLimit;
    this.offset = 0;
  }
}
