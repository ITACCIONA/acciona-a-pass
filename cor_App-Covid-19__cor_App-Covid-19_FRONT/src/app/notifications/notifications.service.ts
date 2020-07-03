import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ConfigService } from '@commons';
import { Observable } from 'rxjs';

interface INotificationsForm {
  target: string;
  country: string;
  division: string;
  workcenter: string;
  interacciona: boolean;
  status: number;
  title: string;
  comment: string;
}

@Injectable({
  providedIn: 'root'
})
export class NotificationsService {
  constructor(private http: HttpClient, private config: ConfigService) {}

  private getDate() {
    const now = new Date();
    return now.toISOString().split('.')[0] + 'Z';
  }

  sendToAll({ title, comment, interacciona, status }: INotificationsForm): Observable<any> {
    return this.http.post(this.config.getUrl('alerts.sendToAll'), {
      title,
      comment,
      interAcciona: interacciona,
      idEstado: status,
      currentDeviceDateTime: this.getDate()
    });
  }

  sendToCountry({ title, comment, country, interacciona, status }: INotificationsForm): Observable<any> {
    return this.http.post(this.config.getUrl('alerts.sendToCountry'), {
      title,
      comment,
      pais: country,
      interAcciona: interacciona,
      idEstado: status,
      currentDeviceDateTime: this.getDate()
    });
  }

  sendToDivision({ title, comment, division, interacciona, status }: INotificationsForm): Observable<any> {
    return this.http.post(this.config.getUrl('alerts.sendToDivision'), {
      title,
      comment,
      idDivision: division,
      interAcciona: interacciona,
      idEstado: status,
      currentDeviceDateTime: this.getDate()
    });
  }

  sendToLocation({ title, comment, workcenter, interacciona, status }: INotificationsForm): Observable<any> {
    return this.http.post(this.config.getUrl('alerts.sendToLocation'), {
      title,
      comment,
      idLocation: workcenter,
      interAcciona: interacciona,
      idEstado: status,
      currentDeviceDateTime: this.getDate()
    });
  }

  sendSingle(title: string, comment: string, employeeId: number): Observable<any> {
    return this.http.post(this.config.getUrl('alerts.single', { employeeId }), {
      title,
      comment,
      currentDeviceDateTime: this.getDate()
    });
  }
}
