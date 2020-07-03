import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ConfigService } from '@commons';
import moment from 'moment';
import { BehaviorSubject, Observable, of, throwError } from 'rxjs';
import { map, tap } from 'rxjs/operators';
import { MastersService } from '../masters.service';
import { IHeaderNamesService } from '../ui/header/header.service';
import {
  GenericResponse,
  IEmployee,
  IEmployeeList,
  IEmployeePersonal,
  IMedicalMonitoring,
  IMedicalMonitoringResp,
  IResponsibleStatement,
  IResponsibleStatementResp,
  IRiskFactors,
  IRiskFactorsResp,
  ITemperature,
  ITemperatureResp,
  ITests
} from './interfaces';

@Injectable({
  providedIn: 'root'
})
export class EmployeeService implements IHeaderNamesService {
  private currentEmployeeName = new BehaviorSubject<string>(null);

  constructor(private http: HttpClient, private config: ConfigService, private masters: MastersService) {}

  getBreadCrumb(): Observable<string> {
    return this.currentEmployeeName;
  }

  getPersonal(employeeId: string): Observable<IEmployeePersonal> {
    return this.http
      .get<GenericResponse<IEmployeePersonal>>(this.config.getUrl('employees.personal', { employeeId }))
      .pipe(map(r => r.data[0]));
  }

  getPassport(employeeId: string): Observable<IEmployee> {
    return this.http.get<GenericResponse<IEmployee>>(this.config.getUrl('employees.passport', { employeeId })).pipe(
      tap(res => {
        this.currentEmployeeName.next(res.data[0].nombreEmpleado);
      }),
      map(r => r.data[0])
    );
  }

  savePassportState(idPassportState: number, idEmployee: number) {
    if (isNaN(idPassportState) || isNaN(idEmployee)) {
      return of(false);
    } else {
      return this.http.post(this.config.getUrl('employees.changePassport', { employeeId: idEmployee }), {
        idEmployee,
        currentDeviceDateTime: moment().format(),
        idPassportState
      });
    }
  }

  goRedPassport(employeeId: number | string) {
    const role = this.masters.getRole();
    let endpoint;

    if (role === 'PRL' || role === 'GestorContratas') {
      endpoint = 'employees.prlRedPassport';
    }
    if (role === 'RRHHDesc') {
      endpoint = 'employees.hrRedPassport';
    }

    if (!endpoint) {
      return throwError(null);
    }
    return this.http.post(this.config.getUrl(endpoint, { employeeId }), {
      currentDeviceDateTime: moment().format()
    });
  }

  expirePassport(employeeId: number | string) {
    return this.http.post(this.config.getUrl('employees.expirePassport', { employeeId }), {
      currentDeviceDateTime: moment().format()
    });
  }

  exportList(
    sortOrder: string,
    nombre: string,
    orderByDescending: string | boolean,
    filter: any = {}
  ): Observable<any> {
    const body = {
      sortOrder,
      nombre,
      orderByDescending,
      ...filter
    };

    return this.http.post<GenericResponse<IEmployeeList>>(this.config.getUrl('employees.export'), body, {
      responseType: 'blob' as 'json'
    });
  }

  getList(
    sortOrder: string,
    nombre: string,
    orderByDescending: string | boolean,
    page: number,
    filter: any = {}
  ): Observable<IEmployeeList> {
    const body = {
      sortOrder,
      nombre,
      orderByDescending,
      page,
      ...filter
    };
    return this.http
      .post<GenericResponse<IEmployeeList>>(this.config.getUrl('employees.list'), body)
      .pipe(map(r => r.data[0]));
  }

  getRiskFactors(employeeId: string): Observable<IRiskFactors[]> {
    return this.http
      .get<GenericResponse<IRiskFactorsResp>>(this.config.getUrl('employees.riskFactors', { employeeId }))
      .pipe(map(r => r.data[0].valoracionFactorRiesgos));
  }

  getResponsibleStatement(employeeId: string): Observable<IResponsibleStatement[]> {
    return this.http
      .get<GenericResponse<IResponsibleStatementResp>>(
        this.config.getUrl('employees.responsibleStatement', { employeeId })
      )
      .pipe(map(r => r.data[0].symptomsByDay));
  }

  getTests(employeeId: string): Observable<ITests> {
    return this.http
      .get<GenericResponse<ITests>>(this.config.getUrl('employees.tests', { employeeId }))
      .pipe(map(r => r.data[0]));
  }

  getMonitoring(employeeId: string): Observable<IMedicalMonitoring[]> {
    return this.http
      .get<GenericResponse<IMedicalMonitoringResp>>(this.config.getUrl('employees.monitoring', { employeeId }))
      .pipe(map(r => r.data[0].medicalMonitoring));
  }

  getTemperatures(employeeId: string): Observable<ITemperature[]> {
    return this.http
      .get<GenericResponse<ITemperatureResp>>(this.config.getUrl('employees.temperature', { employeeId }))
      .pipe(map(r => r.data[0].meditions));
  }

  registerFever(employeeId: string, date): Observable<any> {
    const data = {
      isTemperatureOverThreshold: true,
      meditionDateTime: date
    };
    return this.http.post(this.config.getUrl('employees.temperature', { employeeId }), data);
  }

  addMonitoring(employeeId: string, date, type, params): Observable<any> {
    const data = {
      idParameterType: type,
      parametros: params,
      fechaSeguimiento: date
    };
    return this.http.post(this.config.getUrl('employees.addMonitoring', { employeeId }), data);
  }

  removeTest(employeeId: number, testId: number, testType: string): Observable<any> {
    employeeId.toString();
    testId.toString();
    if (testType === 'TESTS.RAPID') {
      return this.http.delete(this.config.getUrl('employees.removeQuickTest', { employeeId, testId }));
    } else if (testType === 'TESTS.PCR') {
      return this.http.delete(this.config.getUrl('employees.removePcrTest', { employeeId, testId }));
    } else {
      return this.http.delete(this.config.getUrl('employees.removeMonitoring', { employeeId, testId }));
    }
  }
}
