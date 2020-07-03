import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { IAppRoles, MastersService } from './masters.service';

const DEFAULT_ROUTES: { [key in IAppRoles]: string } = {
  Comunicacion: 'notifications',
  ServicioMedico: 'list',
  PRL: 'list',
  RRHHCent: 'list',
  RRHHDesc: 'list',
  GestorContratas: 'outsourcing'
};

@Injectable({
  providedIn: 'root'
})
export class RouteRedirectGuard implements CanActivate {
  constructor(private masters: MastersService, private router: Router) {}
  canActivate(next: ActivatedRouteSnapshot, state: RouterStateSnapshot): Promise<boolean | UrlTree> {
    return new Promise((resolve, reject) => {
      this.masters.ready.subscribe(r => {
        if (r) {
          const allowed = next.data.roles;
          if (allowed.indexOf(this.masters.getRole()) === -1) {
            return this.router.navigate([DEFAULT_ROUTES[this.masters.getRole()]]).then(() => resolve(false));
          } else {
            resolve(true);
          }
        }
      });
    });
  }
}
