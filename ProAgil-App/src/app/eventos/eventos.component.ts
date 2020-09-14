import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-eventos',
  templateUrl: './eventos.component.html',
  styleUrls: ['./eventos.component.scss']
})
export class EventosComponent implements OnInit {

    eventos: any;

    constructor(private http: HttpClient) { }

    // tslint:disable-next-line:typedef
    ngOnInit() {
      this.getEventos();
  }

  // tslint:disable-next-line:typedef
  getEventos(){
    this.http.get('http://localhost:5000/Evento').subscribe(response => {
      this.eventos = response;
      console.log(this.eventos);
    }, error => {
      console.log(error);
    });
  }
}
