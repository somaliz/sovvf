import { Component, OnInit } from '@angular/core';
import { ChatRowComponent } from "../chat-row/chat-row.component";

@Component({
    selector: 'app-chat',
    templateUrl: './chat.component.html',
    styleUrls: ['./chat.component.scss']
})
export class ChatComponent implements OnInit {
    constructor() { }

    righe: ChatRowComponent[] = [];

    ngOnInit() {
        this.righe.push(new ChatRowComponent(), 
                        new ChatRowComponent());
    }


}
