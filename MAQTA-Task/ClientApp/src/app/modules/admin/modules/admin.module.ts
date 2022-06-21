import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AdminRoutingModule } from './admin-routing.module';
import { AdminComponent } from '../components/admin/admin.component';

@NgModule({
  declarations: [AdminComponent],
  imports: [CommonModule, AdminRoutingModule],
})
export class AdminModule {}
