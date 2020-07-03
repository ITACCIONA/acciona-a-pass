import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { ConfigService } from './config.service';

@NgModule({
  imports: [CommonModule],
  providers: [ConfigService]
})
export class ConfigModule {}

@NgModule({
  imports: [CommonModule, ConfigModule]
})
export class ConfigTestingModule {}
