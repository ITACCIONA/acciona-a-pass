import { Component, Input, OnInit } from '@angular/core';
import { EmployeeService } from '../../employee.service';

@Component({
  selector: 'app-risk-factors',
  templateUrl: './risk-factors.component.html',
  styleUrls: ['./risk-factors.component.scss']
})
export class RiskFactorsComponent implements OnInit {
  @Input()
  id: string;

  isLoading = true;

  factors = [];

  constructor(private service: EmployeeService) {}

  ngOnInit(): void {
    this.service.getRiskFactors(this.id).subscribe(r => {
      this.factors = r.filter(rk => rk.value === true).map(f => ({
        icon: this.getIcon(f.name),
        name: `RISKFACTORS.${f.name.toUpperCase()}`
      }));
      this.isLoading = false;
    });
  }

  private getIcon(risk: string) {
    switch (risk.toUpperCase()) {
      case 'ALTAEXPOSICION':
        return 'medical';
      case 'VULNERABLES':
        return 'risk';
      case 'POSITIVO':
        return 'covid-19';
    }
  }
}
