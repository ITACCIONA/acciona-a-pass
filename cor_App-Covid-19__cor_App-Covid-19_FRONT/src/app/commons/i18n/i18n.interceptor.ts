import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { I18NEXT_SERVICE, ITranslationService } from 'angular-i18next';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class I18NextInterceptor implements HttpInterceptor {
  constructor(@Inject(I18NEXT_SERVICE) private i18NextService: ITranslationService) {}

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const lang: string = this.i18NextService.language;

    let request = req;

    if (lang) {
      request = req.clone({
        setHeaders: {
          'Accept-Language': lang
        }
      });
    }

    return next.handle(request);
  }
}
