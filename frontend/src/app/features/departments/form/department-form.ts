import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { CardModule } from 'primeng/card';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { SelectModule } from 'primeng/select';
import { RippleModule } from 'primeng/ripple';

@Component({
  selector: 'app-department-form',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule, CardModule, ButtonModule, InputTextModule, SelectModule, RippleModule],
  templateUrl: './department-form.html',
  styleUrl: './department-form.css'
})
export class DepartmentFormComponent {
  department = {
    name: '',
    manager: null as any
  };

  managers = [
    { label: 'Dr. John Smith', value: 1 },
    { label: 'Dr. Jane Johnson', value: 2 },
    { label: 'Dr. Michael Williams', value: 3 }
  ];

  onSubmit() {
    console.log('Form submitted:', this.department);
  }
}

