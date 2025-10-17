import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { TiptapEditor } from './tiptap-editor/tiptap-editor';
import { NgxTexteditor } from "./ngx-texteditor/ngx-texteditor";
import { Dashboard } from "./dashboard/dashboard";

@Component({
  selector: 'app-root',
  imports: [Dashboard, RouterOutlet],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  protected readonly title = signal('demo');
}
