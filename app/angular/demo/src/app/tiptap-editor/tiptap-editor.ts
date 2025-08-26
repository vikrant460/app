import { CommonModule } from '@angular/common';
import { Component, OnDestroy, ViewEncapsulation } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Editor } from '@tiptap/core';
import { FloatingMenu } from '@tiptap/extension-floating-menu';
import Link from '@tiptap/extension-link';
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
  output = '';
  private paragraphDoc = {
    "type": "doc",
    "content": [
      {
        "type": "paragraph",
        "content": [
          {
            "type": "text",
            "text": "Hello Tiptap!"
          }
        ]
      }
    ]
  }
  private linkDoc = {
  type: 'doc',
  content: [
    {
      type: 'paragraph',
      content: [
        {
          type: 'text',
          text: 'Example',
          marks: [
            {
              type: 'link',
              attrs: {
                href: 'https://example.com',
              },
            },
          ],
        },
      ],
    },
  ],
}
  showFloatingMenu = false;
  editor = new Editor({
    content: this.paragraphDoc,
    extensions: [StarterKit, FloatingMenu],
    onUpdate: ({ editor }) => {
    const json = editor.getJSON();
 
    this.output = JSON.stringify(JSON);
       console.log(this.output)
  },
  });
  addLink() {
    const url = prompt('Enter the URL');

    if (url && this.isValidUrl(url)) {
      // this.editor
      //   .chain()
      //   .focus()
      //   .extendMarkRange('link')
      //   .setLink({ href: url })
      //   .run();
      this.editor.chain().focus().setLink({ href: url }).run();
    }
  }
  isActive(format: string) {
    return this.editor.isActive(format);
  }

  private isValidUrl(str: string) {
    try {
      new URL(str);
      return true;
    } catch {
      return false;
    }
  }
  insertText(event: Event) {
    const text = (event.target as HTMLTextAreaElement).value;
    if (!text || !this.editor?.isEditable) return
    this.editor.chain().focus().insertContent(text).run()
  }
  ngOnDestroy(): void {
    this.editor.destroy();
  }
}
