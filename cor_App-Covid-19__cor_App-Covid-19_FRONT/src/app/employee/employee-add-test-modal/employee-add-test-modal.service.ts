import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ConfigService } from '@commons';
import { Observable, of as observableOf } from 'rxjs';

interface ModalDataReques {
  date: Date;
  type: 'fast' | 'pcr';
  fast: { [key: string]: string };
  pcr: { [key: string]: string };
}

@Injectable()
export class EmployeeAddTestModalService {
  constructor(private http: HttpClient, private config: ConfigService) {}

  saveFastTest(employeeId: number, request: ModalDataReques): Observable<Object> {
    if (!request) {
      return observableOf(false);
    } else {
      if (request.type === 'fast') {
        return this.http.post(this.config.getUrl('employees.postQuickTest', { employeeId }), {
          control: Boolean(Number(request.fast.control)),
          igg: Boolean(Number(request.fast.igg)),
          igm: Boolean(Number(request.fast.igm)),
          fechaTest: request.date.toISOString()
        });
      } else if (request.type === 'pcr') {
        return this.http.post(this.config.getUrl('employees.postPcrTest', { employeeId }), {
          positivo: Boolean(Number(request.pcr.pcr)),
          fechaTest: request.date.toISOString()
        });
      }
    }
    return observableOf(false);
  }
}
