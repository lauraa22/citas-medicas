import { Signal, WritableSignal, computed, effect, signal } from '@angular/core';

type WithId = { id: number };

export abstract class BaseStore<T extends WithId> {
  private readonly state: WritableSignal<T[]>;
  readonly items: Signal<T[]>;
  readonly total: Signal<number>;

  protected constructor(
    private readonly storageKey: string,
    initialData: T[],
  ) {
    this.state = signal<T[]>(this.read(initialData));
    this.items = this.state.asReadonly();
    this.total = computed(() => this.state().length);

    effect(() => {
      if (typeof localStorage !== 'undefined') {
        localStorage.setItem(this.storageKey, JSON.stringify(this.state()));
      }
    });
  }

  private read(initialData: T[]): T[] {
    if (typeof localStorage === 'undefined') return initialData;
    const raw = localStorage.getItem(this.storageKey);
    if (!raw) return initialData;
    try {
      return JSON.parse(raw) as T[];
    } catch {
      return initialData;
    }
  }

  getById(id: number): T | undefined {
    return this.state().find((x) => x.id === id);
  }
  create(item: T): void {
    this.state.update((list) => [...list, item]);
  }
  update(item: T): void {
    this.state.update((list) => list.map((x) => (x.id === item.id ? item : x)));
  }
  delete(id: number): void {
    this.state.update((list) => list.filter((x) => x.id !== id));
  }
  nextId(): number {
    return this.state().length ? Math.max(...this.state().map((x) => x.id)) + 1 : 1;
  }
  reset(data: T[]): void {
    this.state.set(data);
  }
}
