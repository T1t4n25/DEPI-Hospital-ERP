import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { CardModule } from 'primeng/card';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { InputNumberModule } from 'primeng/inputnumber';
import { SelectModule } from 'primeng/select';
import { RippleModule } from 'primeng/ripple';

@Component({
  selector: 'app-service-form',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule, CardModule, ButtonModule, InputTextModule, InputNumberModule, SelectModule, RippleModule],
  templateUrl: './service-form.html',
  styleUrl: './service-form.css'
})
export class ServiceFormComponent {
  service = {
    name: '',
    cost: null as number | null,
    department: null as any
  };

  departments = [
    { label: 'General Medicine', value: 1 },
    { label: 'Cardiology', value: 2 },
    { label: 'Radiology', value: 3 }
  ];

  onSubmit() {
    console.log('Form submitted:', this.service);
  }
}

