import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Email } from 'src/app/models/email.model';
import { EmailService } from 'src/app/services/email.service';

@Component({
  selector: 'app-contact',
  templateUrl: './contact.component.html',
  styleUrls: ['./contact.component.scss']
})
export class ContactComponent {

  form: FormGroup

  constructor(private fb: FormBuilder, private emailService: EmailService)
  {
    this.form = fb.group({
      nombre: ['', Validators.required],
      remitente: ['', [Validators.required, Validators.email]],
      asunto: ['', Validators.required],
      contenido: ['', Validators.required]
    })
  }

  sendEmail(){

    const email: Email = {
      nombre: this.form.value.nombre,
      remitemte: this.form.value.remitente,
      asunto: this.form.value.asunto,
      contenido: this.form.value.contenido
    }

    this.emailService.sendEmail(email).subscribe(data => {
      console.log(data);
    })
  }
}
