import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ConfigService } from '@commons';
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { GenericResponse } from './employee/interfaces';

export interface IMasterConfig {
  idParameterType: number;
  name: string;
  parameters: { idParameter: number; name: string }[];
}

export interface IPassportState {
  id: number;
  name: string;
  color: string;
  exp: boolean;
}

export interface UserInfo {
  idEmpleado: number;
  nombreUsuario: string;
  inicialesUsuario: string;
  roleName: string[];
}

export interface ILocation {
  idLocation: number;
  name: string;
  ciudad: string;
  codPostasl: string;
  direccion: string;
  pais: string;
}
export interface ICountry {
  name: string;
}
export interface IDivision {
  name: string;
  idDivision: number;
}
export interface ICity {
  ciudad: string;
  pais: string;
}

export interface IRegion {
  id: number;
  name: string;
  idCountry: number;
  country: string;
}
export interface IArea {
  id: number;
  name: string;
  idRegion: number;
}

export interface ISuperior {
  id: number;
  completeName: string;
}

type readyStates = boolean | 'failed';

export type IAppRoles = 'PRL' | 'ServicioMedico' | 'Comunicacion' | 'RRHHCent' | 'RRHHDesc' | 'GestorContratas';
const allowedRoles = ['ServicioMedico', 'PRL', 'Comunicacion', 'RRHHCent', 'RRHHDesc', 'GestorContratas'];

@Injectable({
  providedIn: 'root'
})
export class MastersService {
  ready = new BehaviorSubject<readyStates>(false);

  analytics = new BehaviorSubject<IMasterConfig>(null);
  images = new BehaviorSubject<IMasterConfig>(null);
  passportStates: IPassportState[];
  userInfo: UserInfo;

  constructor(private http: HttpClient, private config: ConfigService) {
    this.fetchMonitoring();
    this.fetchPassportStates();
  }

  getRole(): IAppRoles {
    if (this.userInfo) {
      const intersect = this.userInfo.roleName.filter(value => allowedRoles.includes(value)) as IAppRoles[];
      const role = intersect[0];
      return role;
    }
    return null;
  }

  errNotMsal(err) {
    if (!err.message.includes('User login is required')) {
      this.ready.next('failed');
    }
  }

  login() {
    return this.http.get<GenericResponse<UserInfo>>(this.config.getUrl('login')).subscribe(
      res => {
        if (res.data.length > 0) {
          let found = false;
          if (res.data[0].roleName.some(el => allowedRoles.indexOf(el) !== -1)) {
            found = true;
          }
          if (found) {
            this.userInfo = res.data[0];
          }
          this.ready.next(found);
        } else {
          this.ready.next('failed');
        }
      },
      err => this.errNotMsal(err)
    );
  }

  fetchPassportStates() {
    this.http
      .get<GenericResponse<IPassportState>>(this.config.getUrl('masters.statuses'))
      .pipe(map(r => r.data))
      .subscribe(
        m => {
          this.passportStates = m;
        },
        err => this.errNotMsal(err)
      );
  }

  fetchPassportStatesSimple(): Observable<IPassportState[]> {
    return this.http
      .get<GenericResponse<IPassportState>>(this.config.getUrl('masters.statusesHR'))
      .pipe(map(r => r.data));
  }

  fetchMonitoring() {
    this.http
      .get<GenericResponse<IMasterConfig>>(this.config.getUrl('masters.monitoring'))
      .pipe(map(r => r.data))
      .subscribe(
        m => {
          this.analytics.next(m.find(p => p.name === 'Analitica'));
          this.images.next(m.find(p => p.name === 'Imagen'));
        },
        err => this.errNotMsal(err)
      );
  }

  fetchCountries(): Observable<ICountry[]> {
    return this.http.get<GenericResponse<ICountry>>(this.config.getUrl('masters.countries')).pipe(map(r => r.data));
  }

  fetchDivisions(): Observable<IDivision[]> {
    return this.http.get<GenericResponse<IDivision>>(this.config.getUrl('masters.divisions')).pipe(map(r => r.data));
  }

  fetchLocations(): Observable<ILocation[]> {
    return this.http.get<GenericResponse<ILocation>>(this.config.getUrl('masters.locations')).pipe(map(r => r.data));
  }

  fetchRegions(): Observable<IRegion[]> {
    return this.http.get<GenericResponse<IRegion>>(this.config.getUrl('masters.regions')).pipe(map(r => r.data));
  }

  fetchAreas(): Observable<IArea[]> {
    return this.http.get<GenericResponse<IArea>>(this.config.getUrl('masters.areas')).pipe(map(r => r.data));
  }

  fetchCities(): Observable<ICity[]> {
    return this.http.get<GenericResponse<ILocation>>(this.config.getUrl('masters.cities')).pipe(map(r => r.data));
  }

  fetchStatuses(): Observable<IPassportState[]> {
    return this.http
      .get<GenericResponse<IPassportState>>(this.config.getUrl('masters.statuses'))
      .pipe(map(r => r.data));
  }

  searchEmployees(filter: string): Observable<ISuperior[]> {
    return this.http
      .get<GenericResponse<any>>(this.config.getUrl('masters.employees'), { params: { filter } })
      .pipe(map(r => r.data.map(v => ({ id: v.id, completeName: `${v.nombre} ${v.apellidos}` }))));
  }
}
