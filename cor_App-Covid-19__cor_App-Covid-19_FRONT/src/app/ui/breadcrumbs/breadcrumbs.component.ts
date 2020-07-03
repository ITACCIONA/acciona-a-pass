import { Component, OnInit } from '@angular/core';
import { HeaderService } from '../header/header.service';

@Component({
  selector: 'app-breadcrumbs',
  templateUrl: './breadcrumbs.component.html',
  styleUrls: ['./breadcrumbs.component.scss']
})
export class BreadcrumbsComponent implements OnInit {
  constructor(private service: HeaderService) {}

  ngOnInit(): void {}

  get breadcrumbs$() {
    return this.service.breadcrumbs$;
  }
}
