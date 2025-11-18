import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { CardModule } from 'primeng/card';
import { ButtonModule } from 'primeng/button';

@Component({
  selector: 'app-medical-record-detail',
  standalone: true,
  imports: [CommonModule, RouterModule, CardModule, ButtonModule],
  templateUrl: './medical-record-detail.html',
  styleUrl: './medical-record-detail.css'
})
export class MedicalRecordDetailComponent {
  record = {
    id: 1,
    patient: 'John Doe',
    doctor: 'Dr. Smith',
    diagnosis: 'Hypertension',
    date: '2024-01-15',
    treatment: 'Prescribed medication and lifestyle changes',
    notes: 'Patient should follow up in 2 weeks'
  };
}

