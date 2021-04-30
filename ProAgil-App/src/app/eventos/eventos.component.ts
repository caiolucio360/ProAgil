import { Component, OnInit, TemplateRef } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { Evento } from '../_models/Evento';
import { EventoService } from '../_services/evento.service';
import { defineLocale } from 'ngx-bootstrap/chronos';
import { ptBrLocale } from 'ngx-bootstrap/locale';
import { BsLocaleService } from 'ngx-bootstrap/datepicker';
import { Template } from '@angular/compiler/src/render3/r3_ast';
defineLocale('pt-br', ptBrLocale);

@Component({
  selector: 'app-eventos',
  templateUrl: './eventos.component.html',
  styleUrls: ['./eventos.component.scss']
})
export class EventosComponent implements OnInit {

  eventos: Evento[];
  evento: Evento;
  imagemLargura = 50;
  imagemMargem = 2;
  mostarImagem = true;
  eventosFiltrados: Evento[];
  registerForm: FormGroup;
  modoSalvar = 'post';
  bodyDeletarEvento = '';
  _filtroLista: string;

  constructor(
    private eventoService: EventoService,
    private modalService: BsModalService,
    private fb: FormBuilder,
    private localeService: BsLocaleService
  ) {
    this.localeService.use('pt-br');
  }

  ngOnInit() {
    this.getEventos() 
       
    this.registerForm = this.fb.group({
      tema: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(50)]],
      local: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(50)]],
      dataEvento: ['', Validators.required],
      qtdPessoas: ['', [Validators.required, Validators.maxLength(5), Validators.pattern("^[0-9]*$")]],
      imagemUrl: ['', [Validators.required]],
      telefone: ['', [Validators.required]],
      email: ['', [Validators.required, Validators.email]]
    })
  }

 
  get filtroLista(): string {
    return this._filtroLista;
  }
  set filtroLista(value: string) {
    this._filtroLista = value;
    this.eventosFiltrados = this.filtroLista ? this.filtrarEventos(this.filtroLista) : this.eventos;
  }

  AbriImagem(){
    console.log('aaa')
  }

  editEvento(evento: Evento, template: any) {
    this.modoSalvar = 'put'
    this.openModal(template);
    this.evento = evento;
    this.registerForm.patchValue(evento);
  }

  excluirEvento(evento: Evento, template: any) {
    this.openModal(template);
    this.evento = evento;
    this.bodyDeletarEvento = `Tem certeza que deseja excluir o Evento: ${evento.tema}, Código: ${evento.id}`;
  }

  confirmeDelete(template: any) {
    this.eventoService.deleteEvento(this.evento.id).subscribe(
      () => {
          template.hide();
          this.getEventos();
        }, error => {
          console.log(error);
        }
    );
  }

  novoEvento(template: any) {
    this.modoSalvar = 'post'
    this.openModal(template);
  }

  openModal(template: any) {
    this.registerForm.reset();
    template.show();
  }

  salvarAlteracao(template: any) {
    if (this.registerForm.valid) {
      if (this.modoSalvar === 'post') {
        this.evento = Object.assign({}, this.registerForm.value);
        this.eventoService.postEvento(this.evento).subscribe(
          (novoEvento: Evento) => {
            console.log(novoEvento);
            template.hide();
            this.getEventos();
          }, error => {
            console.log(error);
          }
        );
      } else {
        this.evento = Object.assign({ id: this.evento.id }, this.registerForm.value);
        this.eventoService.putEvento(this.evento).subscribe(
          (novoEvento: Evento) => {
            console.log(novoEvento);
            template.hide();
            this.getEventos();
          }, error => {
            console.log(error);
          }
        );
      }
    }
  }

  validation() {
    
  }

  alternarImagem() {
    this.mostarImagem = !this.mostarImagem;
  }

  filtrarEventos(filtrarpor: string): Evento[] {
    filtrarpor = filtrarpor.toLowerCase();
    return this.eventos.filter(evento => evento.tema.toLowerCase().indexOf(filtrarpor) !== -1);
  }

  getEventos() {
    this.eventoService.getAllEvento().subscribe((_eventos: Evento[]) => {
      this.eventos = _eventos;
      this.eventosFiltrados = this.eventos;
      console.log(_eventos);
    }, error => {
      console.log(error);
    });
  }

}
