import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-cliente-corpo',
  templateUrl: './cliente-corpo.component.html',
  styleUrls: ['./cliente-corpo.component.scss']
})
export class ClienteCorpoComponent implements OnInit {
  @Input() cpfcnpj: string;
  constructor() { }

  ngOnInit() {
  }

}
