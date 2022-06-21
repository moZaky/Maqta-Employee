import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { EmployeeRoutingModule } from './employee-routing.module';
import { ProfileComponent } from '../components/profile/profile.component';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from 'src/app/app-routing.module';

@NgModule({
  declarations: [ProfileComponent],
  imports: [
    CommonModule,
    EmployeeRoutingModule,

    FormsModule,
    ReactiveFormsModule,
  ],
})
export class EmployeeModule {}
