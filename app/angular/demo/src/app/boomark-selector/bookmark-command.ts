import { EditorState, Transaction, TextSelection } from 'prosemirror-state';
import { Command } from 'prosemirror-state';

export function insertBookmark(bookmarkType: string): Command {
  return function (state: EditorState, dispatch?: (tr: Transaction) => void): boolean {
    const { schema, selection, tr } = state;
    const { from, to } = selection;

    const markType = schema.marks['bookmark'];
    if (!markType) return false;

    const bookmarkText = `${bookmarkType}`;
    const textNode = schema.text(bookmarkText, [markType.create({ type: bookmarkType })]);

    if (dispatch) {
      dispatch(
        tr.replaceRangeWith(from, to, textNode)
          .setSelection(TextSelection.create(tr.doc, from + bookmarkText.length))
      );
    }

    return true;
  };
}


