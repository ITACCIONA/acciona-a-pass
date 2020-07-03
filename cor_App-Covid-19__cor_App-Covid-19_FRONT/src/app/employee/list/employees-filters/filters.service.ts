import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class FiltersService {
  public current = new BehaviorSubject<any>({});

  constructor() {}

  set(newFilter: any) {
    this.current.next({
      divisiones: newFilter.division || [],
      localizaciones: newFilter.location || [],
      paises: newFilter.countries.map(c => c.name) || [],
      regiones: newFilter.regions.map(r => r.id) || [],
      areas: newFilter.areas || []
    });
  }

  clean() {
    this.current.next({});
  }
}
