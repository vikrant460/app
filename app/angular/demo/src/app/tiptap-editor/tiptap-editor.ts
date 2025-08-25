import { CommonModule } from '@angular/common';
import { Component, OnDestroy, ViewEncapsulation } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Editor } from '@tiptap/core';
import { FloatingMenu } from '@tiptap/extension-floating-menu';
import { ListKit } from '@tiptap/extension-list';
import StarterKit from '@tiptap/starter-kit';
import { TiptapEditorDirective, TiptapFloatingMenuDirective } from 'ngx-tiptap';

@Component({
  selector: 'app-tiptap-editor',
  imports: [CommonModule, FormsModule, TiptapEditorDirective, TiptapFloatingMenuDirective],
  templateUrl: './tiptap-editor.html',
  styleUrl: './tiptap-editor.css',
  encapsulation: ViewEncapsulation.None
})
export class TiptapEditor implements OnDestroy {
  private value = '<p>Hello, Tiptap!</p>'; // can be HTML or JSON, see https://www.tiptap.dev/api/editor#content
  showFloatingMenu = false;
  editor = new Editor({
    content: this.value,
    extensions: [StarterKit, FloatingMenu, ListKit]
  });


  insertText(event: Event) {
    const text = (event.target as HTMLTextAreaElement).value;
    if (!text || !this.editor?.isEditable) return
    this.editor.chain().focus().insertContent(text).run()
  }
  ngOnDestroy(): void {
    this.editor.destroy();
  }
}
