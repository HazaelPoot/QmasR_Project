import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Email } from 'src/app/models/email.model';
import { EmailService } from 'src/app/services/email.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-contact',
  templateUrl: './contact.component.html',
  styleUrls: ['./contact.component.scss'],
})
export class ContactComponent {
  form: FormGroup;

  constructor(private fb: FormBuilder, private emailService: EmailService) {
    this.form = fb.group({
      nombre: ['', Validators.required],
      remitente: ['', [Validators.required, Validators.email]],
      asunto: ['', Validators.required],
      contenido: ['', Validators.required],
    });
  }

  sendEmail() {
    const email: Email = {
      nombre: this.form.value.nombre,
      remitemte: this.form.value.remitente,
      asunto: this.form.value.asunto,
      contenido: this.form.value.contenido,
    };

    if (email) {
      Swal.fire({
        title: 'Enviar Correo',
        text: '¿Esta seguro de enviar el mensaje que ha redactado?',
        icon: 'warning',
        showCancelButton: true,
        cancelButtonColor: '#d33',
        cancelButtonText: 'Cancelar',
        confirmButtonColor: '#3085d6',
        confirmButtonText: 'Si, enviar!',
      }).then((result) => {
        if (result) {
          this.emailService.sendEmail(email).subscribe((data) => {}),
            Swal.fire({
              title: 'Enviado',
              text: `Recibirá la respuesta al correo ${email.remitemte}`,
              icon: 'success',
              confirmButtonColor: '#3085d6',
              confirmButtonText: 'OK',
            }).then((result) => {
              if (result.isConfirmed) {
                window.location.reload();
              }
            });
        }
      });
    }
  }
}
