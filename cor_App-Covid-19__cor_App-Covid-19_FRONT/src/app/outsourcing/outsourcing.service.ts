import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { map, tap } from 'rxjs/operators';
import { ConfigService } from '../commons';
import { GenericResponse, IEmployeeList } from '../employee/interfaces';
export interface IOutsourcingEmployee {
  idEmpleado: number;
  nombreEmpleado: string;
  apellidosEmpleado: string;
  dni: string;
  departamento: string;
  nameLocalizacion: string;
  division: string;
  responsable: string;
  idResetear: string;
}

@Injectable({
  providedIn: 'root'
})
export class OutsourcingService {
  private currentEmployeeName = new BehaviorSubject<string>(null);

  constructor(private http: HttpClient, private config: ConfigService) {}

  getBreadCrumb(): Observable<string> {
    return this.currentEmployeeName;
  }

  createEmployee(data): Observable<any> {
    return this.http.post(this.config.getUrl('outsourcing.create'), data);
  }

  getEmployee(employeeId: string): Observable<IOutsourcingEmployee> {
    return this.http
      .get<GenericResponse<IOutsourcingEmployee>>(this.config.getUrl('outsourcing.get', { employeeId }))
      .pipe(
        map(r => r.data[0]),
        tap(res => this.currentEmployeeName.next(`${res.nombreEmpleado} ${res.apellidosEmpleado}`))
      );
  }

  list(
    sortOrder: string,
    nombre: string,
    orderByDescending: boolean,
    page: number,
    filters?: any
  ): Observable<IEmployeeList> {
    const body = {
      sortOrder,
      nombre,
      orderByDescending,
      page,
      ...filters
    };
    return this.http
      .post<GenericResponse<IEmployeeList>>(this.config.getUrl('outsourcing.list'), body)
      .pipe(map(r => r.data[0]));
  }

  export(sortOrder: string, nombre: string, orderByDescending: boolean, filters: any): Observable<any> {
    const body = {
      sortOrder,
      nombre,
      orderByDescending,
      ...filters
    };
    return this.http.post<GenericResponse<any>>(this.config.getUrl('outsourcing.export'), body, {
      responseType: 'blob' as 'json'
    });
  }

  resetPassword(userId: string): Observable<string> {
    return this.http
      .post<{ otp: string }>(this.config.getUrl('identity.resetPassword', null, 'identity'), `"${userId}"`, {
        headers: { 'Content-Type': 'application/json; charset=utf-8' }
      })
      .pipe(map(r => r.otp));
  }
}
