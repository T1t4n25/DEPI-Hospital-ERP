import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { CardModule } from 'primeng/card';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { DatePickerModule } from 'primeng/datepicker';
import { SelectModule } from 'primeng/select';
import { RippleModule } from 'primeng/ripple';

@Component({
  selector: 'app-employee-form',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule, CardModule, ButtonModule, InputTextModule, DatePickerModule, SelectModule, RippleModule],
  templateUrl: './employee-form.html',
  styleUrl: './employee-form.css'
})
export class EmployeeFormComponent {
  employee = {
    firstName: '',
    lastName: '',
    role: null as any,
    department: null as any,
    contactNumber: '',
    hireDate: null as Date | null,
    gender: null as any
  };

  roles = [
    { label: 'Doctor', value: 'Doctor' },
    { label: 'Nurse', value: 'Nurse' },
    { label: 'Receptionist', value: 'Receptionist' },
    { label: 'Pharmacist', value: 'Pharmacist' }
  ];

  departments = [
    { label: 'Cardiology', value: 'Cardiology' },
    { label: 'Pediatrics', value: 'Pediatrics' },
    { label: 'Emergency', value: 'Emergency' },
    { label: 'Orthopedics', value: 'Orthopedics' }
  ];

  genders = [
    { label: 'Male', value: 'Male' },
    { label: 'Female', value: 'Female' }
  ];

  onSubmit() {
    console.log('Form submitted:', this.employee);
  }
}

