import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { CardModule } from 'primeng/card';
import { ButtonModule } from 'primeng/button';
import { DatePickerModule } from 'primeng/datepicker';
import { SelectModule } from 'primeng/select';
import { TextareaModule } from 'primeng/textarea';
import { RippleModule } from 'primeng/ripple';

@Component({
  selector: 'app-medical-record-form',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule, CardModule, ButtonModule, DatePickerModule, SelectModule, TextareaModule, RippleModule],
  templateUrl: './medical-record-form.html',
  styleUrl: './medical-record-form.css'
})
export class MedicalRecordFormComponent {
  record = {
    patient: null as any,
    doctor: null as any,
    diagnosis: null as any,
    date: null as Date | null,
    treatment: '',
    notes: ''
  };

  patients = [
    { label: 'John Doe', value: 1 },
    { label: 'Jane Smith', value: 2 }
  ];

  doctors = [
    { label: 'Dr. Smith', value: 1 },
    { label: 'Dr. Johnson', value: 2 }
  ];

  diagnoses = [
    { label: 'Hypertension', value: 1 },
    { label: 'Common Cold', value: 2 },
    { label: 'Fracture', value: 3 }
  ];

  onSubmit() {
    console.log('Form submitted:', this.record);
  }
}

