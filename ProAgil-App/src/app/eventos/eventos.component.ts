import { Component, OnInit, TemplateRef } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { Evento } from '../_models/Evento';
import { EventoService } from '../_services/evento.service';

@Component({
  selector: 'app-eventos',
  templateUrl: './eventos.component.html',
  styleUrls: ['./eventos.component.scss']
})
export class EventosComponent implements OnInit {

    eventos: Evento[];
    imagemLargura = 50;
    imgagemMargem = 2;
    mostarImagem = false;
    eventosFiltrados: Evento[];
    modalRef: BsModalRef;

    // tslint:disable-next-line:variable-name
    _filtroLista: string;
    get filtroLista(): string {
      return this._filtroLista;
    }
    set filtroLista(value: string){
      this._filtroLista = value;
      this.eventosFiltrados = this.filtroLista ? this.filtrarEventos(this.filtroLista) : this.eventos;
    }

    // tslint:disable-next-line:no-shadowed-variable
    constructor(
      private eventoService: EventoService,
      private modalService: BsModalService
      ) { }

    // tslint:disable-next-line:typedef
    openModal(template: TemplateRef<any>){
      this.modalRef = this.modalService.show(template);
    }
    // tslint:disable-next-line:typedef
    ngOnInit() {
      this.getEventos();
  }
    // tslint:disable-next-line:typedef
    alternarImagem(){
    this.mostarImagem = !this.mostarImagem;
  }

  filtrarEventos(filtrarpor: string): Evento[]{
    filtrarpor = filtrarpor.toLowerCase();
    return this.eventos.filter(evento => evento.tema.toLowerCase().indexOf(filtrarpor) !== -1);
  }

  // tslint:disable-next-line:typedef
  getEventos(){
    // tslint:disable-next-line:variable-name
    this.eventoService.getAllEvento().subscribe((_eventos: Evento[]) => {
      this.eventos = _eventos;
      this.eventosFiltrados = this.eventos;
      console.log(_eventos);
    }, error => {
      console.log(error);
    });
  }
}
