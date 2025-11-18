import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DialogModule } from 'primeng/dialog';
import { ButtonModule } from 'primeng/button';
import { RippleModule } from 'primeng/ripple';

@Component({
  selector: 'app-confirmation-dialog',
  standalone: true,
  imports: [CommonModule, DialogModule, ButtonModule, RippleModule],
  templateUrl: './confirmation-dialog.html',
  styleUrl: './confirmation-dialog.css'
})
export class ConfirmationDialogComponent {
  visible = false;
  message = 'Are you sure you want to proceed?';
  onConfirm = () => {};

  show(message: string, onConfirm: () => void) {
    this.message = message;
    this.onConfirm = onConfirm;
    this.visible = true;
  }

  hide() {
    this.visible = false;
  }

  confirm() {
    this.onConfirm();
    this.hide();
  }
}

