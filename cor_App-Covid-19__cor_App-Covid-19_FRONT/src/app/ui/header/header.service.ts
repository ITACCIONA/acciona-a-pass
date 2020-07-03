import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { BehaviorSubject, Observable, of } from 'rxjs';
import { EmployeeService } from '../../employee/employee.service';
import { OutsourcingService } from '../../outsourcing/outsourcing.service';

export interface BreadCrumb {
  label: Observable<string>;
  url: string;
}

export interface IHeaderNamesService {
  getBreadCrumb(): Observable<string>;
}

@Injectable({
  providedIn: 'root'
})
export class HeaderService {
  public breadcrumbs$: BehaviorSubject<BreadCrumb[]> = new BehaviorSubject<BreadCrumb[]>(null);
  private injector: any;

  constructor(
    private router: Router,
    private employeeService: EmployeeService,
    private outsourcing: OutsourcingService
  ) {
    this.injector = {
      Employee: employeeService,
      Outsourcing: outsourcing
    };
  }

  refresh(routeData) {
    this.breadcrumbs$.next(this.buildBreadCrumb(routeData));
  }

  private buildBreadCrumb(routeData, breadcrumbs: BreadCrumb[] = []): BreadCrumb[] {
    const url = this.router.config.find(r => r.data && r.data.id === routeData.id);
    const breadcrumb = { url: !breadcrumbs.length ? '' : url.path, label: this.transformName(routeData.title) };

    const newBreadcrumbs = [breadcrumb, ...breadcrumbs];
    if (routeData && routeData.parent) {
      const routeParent = this.router.config.find(r => r.data && r.data.id === routeData.parent);
      return this.buildBreadCrumb(routeParent.data, newBreadcrumbs);
    }
    return newBreadcrumbs;
  }

  private transformName(name: string): Observable<string> {
    if (typeof name === 'string' && name.startsWith('::')) {
      const serviceName = name.replace('::', '');
      const service: IHeaderNamesService = this.injector[serviceName];
      return service.getBreadCrumb();
    } else {
      return of(name);
    }
  }
}
