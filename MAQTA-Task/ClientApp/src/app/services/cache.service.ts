import { Injectable, OnDestroy } from '@angular/core';
import { Observable, Subject } from 'rxjs';
import { share } from 'rxjs/operators';
import ls from 'localstorage-slim';
import { environment } from 'src/environments/environment';
import { LocalStorageItem } from '../common/models/local-storage-item';

@Injectable({
  providedIn: 'root',
})
export class CacheService implements OnDestroy {
  private localStorageLangKey: string = environment.localStoragePrefix;
  private onSubject = new Subject<{ key: string; value: any }>();
  public changes = this.onSubject.asObservable().pipe(share());

  constructor() {
    ls.config.encrypt = environment.production;
    ls.config.secret = 206;
    this.start();
  }
  ngOnDestroy(): void {
    this.stop();
  }

  public setPersistanceLocalStorageItem<T>(key: string, value: T): void {
    localStorage.setItem(key, JSON.stringify(value));
  }
  public getPersistanceLocalStorageItem<T>(key: string): T | null {
    var item = localStorage.getItem(key);
    if (item != null) return <T>JSON.parse(item);
    return null;
  }

  public setCustomLocalStorageItem<T>(
    key: string,
    value: LocalStorageItem<T>
  ): void {
    ls.set(`${this.localStorageLangKey}${key}`, JSON.stringify(value));
  }
  public removeItemFromStorage(key: string): void {
    ls.remove(`${this.localStorageLangKey}${key}`);
  }
  public getCustomLocalStorageItem<T>(key: string): LocalStorageItem<T> | null {
    var item = ls.get(`${this.localStorageLangKey}${key}`) as string;
    if (item != null) return <LocalStorageItem<T>>JSON.parse(item);
    return null;
  }

  public setLocalStorageItem(key: string, value: any): void {
    ls.set(`${this.localStorageLangKey}${key}`, JSON.stringify(value));
  }

  public getLocalStorageItem<T>(key: string): T | null {
    var item = ls.get(`${this.localStorageLangKey}${key}`) as string;
    if (item != null) return <T>JSON.parse(item);
    return null;
  }

  public watchStorage(): Observable<any> {
    return this.onSubject.asObservable();
  }

  private storageEventListener(event: StorageEvent) {
    if (event.storageArea == localStorage) {
      let v;
      const eventValue = event.newValue || '';
      try {
        v = JSON.parse(eventValue);
      } catch (e) {
        v = eventValue;
      }
      this.onSubject.next({ key: event.key || '', value: v });
    }
  }
  private start(): void {
    window.addEventListener('storage', this.storageEventListener.bind(this));
  }
  private stop(): void {
    window.removeEventListener('storage', this.storageEventListener.bind(this));
    this.onSubject.complete();
  }
}
