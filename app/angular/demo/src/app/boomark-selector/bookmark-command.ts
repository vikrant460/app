import { EditorState, Transaction } from 'prosemirror-state';

export function insertBookmark(bookmarkType: string) {
  return function (state: EditorState, dispatch?: (tr: Transaction) => void) {
    const { schema, selection } = state;
    const markType = schema.marks['bookmark'];
    if (!markType) return false;

    const bookmarkMark = markType.create({ type: bookmarkType });

    if (selection.empty) {
      // Create a text node with a mark
      const textNode = schema.text(bookmarkType, [bookmarkMark]);
      if (dispatch) {
        dispatch(state.tr.insert(selection.from, textNode).scrollIntoView());
      }
    } else {
      // Apply the mark to the selected text
      if (dispatch) {
        const tr = state.tr.addMark(selection.from, selection.to, bookmarkMark);
        dispatch(tr.scrollIntoView());
      }
    }

    return true;
  };
}
