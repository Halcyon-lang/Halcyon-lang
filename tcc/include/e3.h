/*
   e3.h - public header for applications

   Embeddable Editor Engine (EEE)

   Copyright (C) 2010 Laszlo Menczel

   This is free software distributed under the terms of the GNU
   Lesser General Public License (version 2) augmented with extra
   clauses (see LICENSE.TXT). NO WARRANTY.
*/

#if ! defined E3_H
#define E3_H

//======================================================================
// error codes
//======================================================================

enum
{
  E3_NO_ERROR,           // no error

  E3_ERR_ALLOC,          // memory allocation failed
  E3_ERR_ARG_RANGE,      // argument value out of range
  E3_ERR_BAD_ATTR,       // invalid attribute selector
  E3_ERR_BLK_COL,        // invalid text block columns (start > end)
  E3_ERR_BLK_ROW,        // invalid text block rows (start > end)
  E3_ERR_BAD_BUF,        // invalid buffer handle
  E3_ERR_BAD_CMD,        // key combination is not a command
  E3_ERR_BAD_COMTAB,     // command table not found
  E3_ERR_BAD_KEY,        // invalid (undefined) key combination
  E3_ERR_BAD_LIMIT,      // row limit too low (already exceeded)
  E3_ERR_BUF_EMPTY,      // no text in the buffer
  E3_ERR_BUF_RESIZE,     // buffer resizing failed
  E3_ERR_BUF_SIZE,       // text storage buffer too small
  E3_ERR_CMD_BAD_KEY,    // invalid key as command
  E3_ERR_CMD_KEY_MOD,    // invalid key modifier combinaton
  E3_ERR_CMD_KEY_USED,   // key already used as command
  E3_ERR_CMD_PREF_USED,  // prefix key already used
  E3_ERR_CMD_SELF_INS,   // self inserting key as command
  E3_ERR_CMD_TAB,        // no room for new command table
  E3_ERR_DELETE,         // delete operation cannot be done
  E3_ERR_EMPTY_STR,      // empty string as argument
  E3_ERR_FILE_EMPTY,     // no data in the file
  E3_ERR_FILE_OPEN,      // failed to open file
  E3_ERR_FILE_READ,      // failed when reading from file
  E3_ERR_FILE_WRITE,     // failed when writing to file
  E3_ERR_HAS_DIF,        // buffer already has interface
  E3_ERR_INSERT,         // insert operation cannot be done
  E3_ERR_LIB_INIT,       // library is not initialized
  E3_ERR_MOV_BOT,        // cursor already at bottom of text
  E3_ERR_MOV_END,        // cursor already at end of line
  E3_ERR_MOV_HOME,       // cursor already at start of line
  E3_ERR_MOV_TOP,        // cursor already at top of text
  E3_ERR_NO_DIF,         // buffer has no interface
  E3_ERR_NO_SELECT,      // no buffer selected
  E3_ERR_NO_TEXT,        // no text in the buffer
  E3_ERR_NO_WRAP,        // line wrapping disabled
  E3_ERR_NOT_DYN,        // string or array has statically allocated part
  E3_ERR_NOT_FOUND,      // specified object not found
  E3_ERR_NULL_PTR,       // NULL pointer argument
  E3_ERR_TOO_LARGE,      // size too large
  E3_ERR_WIN_FUNC,       // NULL display function
  E3_ERR_WIN_HEIGHT,     // window height out of range
  E3_ERR_WIN_WIDTH,      // window width out of range

  E3_ERR_NOT_YET,        // not implemented yet
  E3_ERR_UNKNOWN,        // undefined error (internal)
  E3_ERR_MSG_NUM
};

//======================================================================
// action codes
//======================================================================

/*
  Note: The order of command enumeration is significant. Proper command
        processing in function '_e3_do_command' (edit.c) depends on the
        specific order of commadn codes.
*/

enum
{
  /*-------------------------------------------------------------------*/
  /*   code                    description                 default key */
  /*-------------------------------------------------------------------*/

  E3_NO_ACTION,        // zero is invalid
  /* navigation */
  E3_MOV_LEFT,          // left                               left
  E3_MOV_RIGHT,         // right                              right
  E3_MOV_UP,            // up                                 up
  E3_MOV_DOWN,          // down                               down
  E3_MOV_PGUP,          // to previous page                   pgup
  E3_MOV_PGDN,          // to next page                       pgdn
  E3_MOV_HOME,          // to beginning of line               home
  E3_MOV_END,           // to end of line                     end
  E3_MOV_LWORD,         // to previous word                   Ctrl-left
  E3_MOV_RWORD,         // to next word                       Ctrl-right
  E3_MOV_DOCBEG,        // to beginning of text               Ctrl-home
  E3_MOV_DOCEND,        // to end of text                     Ctrl-end
  /* deleting text */
  E3_DEL_PREV,          // delete char before cursor          backspace
  E3_DEL_CURR,          // delete char under cursor           del
  E3_DEL_EOW,           // delete up to end of current word   Alt-Ctrl-w
  E3_DEL_WORD,          // delete current word                Alt-w
  E3_DEL_EOL,           // delete up to end of current line   Alt-Ctrl-l
  E3_DEL_LINE,          // delete current line                Alt-l
  E3_DEL_SELECT,        // delete marked text                 Alt-Ctrl-m
  E3_DEL_SPACE,         // delete white space                 Alt-Ctrl-s
  /* selecting text */
  E3_SEL_LEFT,          // extend selection to the left       Shift-left
  E3_SEL_RIGHT,         // extend selection to the right      Shift-right
  E3_SEL_UP,            // extend selection upward            Shift-up
  E3_SEL_DOWN,          // extend selection downward          Shift-down
  E3_SEL_EOW,           // mark to end of current word        Shift-Alt-w
  E3_SEL_WORD,          // mark current word as selected      Shift-Ctrl-w
  E3_SEL_BOL,           // mark to start of current line      Shift-home
  E3_SEL_EOL,           // mark to end of current line        Shift-end
  E3_SEL_LINE,          // mark current line                  Shift-Ctrl-l
  E3_SEL_ALL,           // select whole text                  Shift-Ctrl-a
  /* bookmarks */
  E3_MARK_ADD,          // add bookmark at cursor             Ctrl-space
  E3_MARK_BACK,         // to previous bookmark               Alt-pgup
  E3_MARK_FORW,         // to next bookmark                   Alt-pgdn
  E3_MARK_SWAP,         // exchange bookmark and cursor       Alt-x
  /* scrolling */
  E3_SCR_LEFT,          // scroll text left                   Alt-left
  E3_SCR_RIGHT,         // scroll text right                  Alt-right
  E3_SCR_UP,            // scroll text upward                 Alt-up
  E3_SCR_DOWN,          // scroll text downward               Alt-down
  /* clipboard */
  E3_CLIP_COPY,         // copy marked text to paste buffer   Ctrl-c
  E3_CLIP_CUT,          // cut marked text to paste buffer    Ctrl-x
  E3_CLIP_PASTE,        // insert text from the paste buffer  Ctrl-v
  /* modifying case */
  E3_CASE_UPPER,        // word or marked to upper case       Alt-u
  E3_CASE_LOWER,        // word or marked to lower case       Alt-l
  E3_CASE_CAPITAL,      // capitalize word or marked          Alt-c
  /* miscellaneous */
  E3_TXT_NEWLINE,       // start a new line                   enter
  E3_TXT_TRCHAR,        // transpose (move right) character   Alt-t
  E3_TXT_TRLINE,        // transpose (move down) line         Shift-Alt-t
  E3_ADD_BLANK,         // add blank line at cursor           Ctrl-y
  E3_TOG_INSERT,        // toggle insert vs. overwrite        ins
  E3_ACTION_NUM
};

// commands for which no hotkey can be assigned

enum
{
  E3_SELF_INSERT = E3_ACTION_NUM,
  E3_CURSOR_OFF,
  E3_CURSOR_ON
};

//======================================================================
// key codes
//======================================================================

#define E3_MOD_ALT              0x10000000
#define E3_MOD_CTRL             0x20000000
#define E3_MOD_SHIFT            0x40000000

#define E3_MOD_ALT_CTRL         (E3_MOD_ALT | E3_MOD_CTRL)
#define E3_MOD_ALT_SHIFT        (E3_MOD_ALT | E3_MOD_SHIFT)
#define E3_MOD_ALT_CTRL_SHIFT   (E3_MOD_ALT | E3_MOD_CTRL | E3_MOD_SHIFT)
#define E3_MOD_CTRL_SHIFT       (E3_MOD_CTRL | E3_MOD_SHIFT)

enum
{
  E3_KEY_NONE,      // invalid key code, we don't use zero for any key

  /* visible on displays */
  E3_KEY_A,
  E3_KEY_B,
  E3_KEY_C,
  E3_KEY_D,
  E3_KEY_E,
  E3_KEY_F,
  E3_KEY_G,
  E3_KEY_H,
  E3_KEY_I,
  E3_KEY_J, // 10
  E3_KEY_K,
  E3_KEY_L,
  E3_KEY_M,
  E3_KEY_N,
  E3_KEY_O,
  E3_KEY_P,
  E3_KEY_Q,
  E3_KEY_R,
  E3_KEY_S,
  E3_KEY_T,     // 20
  E3_KEY_U,
  E3_KEY_V,
  E3_KEY_W,
  E3_KEY_X,
  E3_KEY_Y,
  E3_KEY_Z,

  E3_KEY_BQUOTE,

  E3_KEY_1,
  E3_KEY_2,
  E3_KEY_3,     // 30
  E3_KEY_4,
  E3_KEY_5,
  E3_KEY_6,
  E3_KEY_7,
  E3_KEY_8,
  E3_KEY_9,
  E3_KEY_0,

  E3_KEY_MINUS,
  E3_KEY_EQUALS,
  E3_KEY_BSLASH,    // 40
  E3_KEY_TAB,
  E3_KEY_LBRACKET,
  E3_KEY_RBRACKET,
  E3_KEY_SEMICOL,
  E3_KEY_QUOTE,
  E3_KEY_COMMA,
  E3_KEY_STOP,
  E3_KEY_SLASH,
  E3_KEY_SPACE,

  /* not printable */
  E3_KEY_ESC,       // 50
  E3_KEY_BSPACE,
  E3_KEY_ENTER,

  E3_KEY_F1,
  E3_KEY_F2,
  E3_KEY_F3,
  E3_KEY_F4,
  E3_KEY_F5,
  E3_KEY_F6,
  E3_KEY_F7,
  E3_KEY_F8,        // 60
  E3_KEY_F9,
  E3_KEY_F10,
  E3_KEY_F11,
  E3_KEY_F12,

  E3_KEY_INS,
  E3_KEY_DEL,
  E3_KEY_HOME,
  E3_KEY_END,
  E3_KEY_PGUP,
  E3_KEY_PGDN,      // 70
  E3_KEY_LEFT,
  E3_KEY_RIGHT,
  E3_KEY_UP,
  E3_KEY_DOWN,      // 74

  E3_KEY_NUM      // for use as array dimension
};

#define E3_KEY_LAST_PRINTABLE   E3_KEY_SPACE

#define E3_KEY_TILDE      E3_KEY_BQUOTE | E3_MOD_SHIFT

#define E3_KEY_EXCLAM     E3_KEY_1 | E3_MOD_SHIFT
#define E3_KEY_AT         E3_KEY_2 | E3_MOD_SHIFT
#define E3_KEY_HASH       E3_KEY_3 | E3_MOD_SHIFT
#define E3_KEY_DOLLAR     E3_KEY_4 | E3_MOD_SHIFT
#define E3_KEY_PERCENT    E3_KEY_5 | E3_MOD_SHIFT
#define E3_KEY_CARET      E3_KEY_6 | E3_MOD_SHIFT
#define E3_KEY_AMPER      E3_KEY_7 | E3_MOD_SHIFT
#define E3_KEY_ASTERISK   E3_KEY_8 | E3_MOD_SHIFT
#define E3_KEY_LPAREN     E3_KEY_9 | E3_MOD_SHIFT
#define E3_KEY_RPAREN     E3_KEY_0 | E3_MOD_SHIFT

#define E3_KEY_UNDER      E3_KEY_MINUS    | E3_MOD_SHIFT
#define E3_KEY_PLUS       E3_KEY_EQUALS   | E3_MOD_SHIFT
#define E3_KEY_PIPE       E3_KEY_BSLASH   | E3_MOD_SHIFT
#define E3_KEY_LBRACE     E3_KEY_LBRACKET | E3_MOD_SHIFT
#define E3_KEY_RBRACE     E3_KEY_RBRACKET | E3_MOD_SHIFT
#define E3_KEY_COLON      E3_KEY_SEMICOL  | E3_MOD_SHIFT
#define E3_KEY_DQUOTE     E3_KEY_QUOTE    | E3_MOD_SHIFT
#define E3_KEY_LESS       E3_KEY_COMMA    | E3_MOD_SHIFT
#define E3_KEY_GREATER    E3_KEY_STOP     | E3_MOD_SHIFT
#define E3_KEY_QUEST      E3_KEY_SLASH    | E3_MOD_SHIFT

//======================================================================
// abstract drawing attributes (color is application controlled)
//======================================================================

#define E3_ATR_NORMAL   1
#define E3_ATR_SELECT   2
#define E3_ATR_CURSOR   4

//======================================================================
// miscellaneous constants
//======================================================================

#define E3_MAX_ERR_LEN    256

enum { E3_POS_LEADING, E3_POS_TRAILING };

/*
  flags to enable logging features:

  CONSOLE = use 'printf'
  FILE = write to logfile 'e3.log'
  TRACE = enable function tracing
  APPEND = do not dlete old logfile
*/

#define E3_LOG_CONSOLE        1
#define E3_LOG_FILE           2
#define E3_LOG_TRACE          4
#define E3_LOG_APPEND         8

//======================================================================
// editor commands
//======================================================================

typedef struct e3cmd_s
{
  int cmd;        // EEE action code
  int key;        // trigger key with modifier flag(s)
  int prefix;     // prefix key with modifier flag (zero = none)
} e3cmd_t;

//======================================================================
// text storage types
//======================================================================

enum { E3_STATIC_ALL, E3_DYNAMIC_DATA, E3_DYNAMIC_ALL };

typedef struct e3str_s
{
  char *data;       // pointer to a string buffer
  int   size;       // number of characters in 'data' (w/o the terminating zero)
  int   lncount;    // number of lines stored in 'data'
  int   maxlen;     // size of longest line substring (0 = unknown)
  int   dynamic;    // 0 = the whole structure is statically allocated
                    // 1 = the string 'data' is dynamically allocated
                    // 2 = the whole structure is dynamically allocated
} e3str_t;

typedef struct e3arr_s
{
  char **data;      // array of pointers to string buffers
  int    arrlen;    // number of allocated string buffers (size of array)
  int    count;     // number of string buffers with valid stored data
  int    len;       // allocated length of string buffers
  int    dynamic;   // 0 = the whole structure is statically allocated
                    // 1 = pointer array and buffers are dynamically allocated
                    // 2 = the whole structure is dynamically allocated
} e3arr_t;

//======================================================================
// display interface type
//======================================================================

/*
  arguments for clearing and drawing:
  -----------------------------------
  atr  = preset drawing colors: NORMAL, SELECT or CURSOR
  arr  = array of string pointers
  chr  = character
  str  = string
  row  = row position in window (line number)
  col  = column position in 'row'
  scol = start column of block
  ecol = end column of block
*/

typedef struct e3dif_s
{
  int         id;       // interface ID (> 0)

  /* window size */
  int         winh;     // height (lines)
  int         winw;     // width (characters)

  /* set attributes */
  void (* setatr)   (int,int);                // (atr,id);

  /* clear */
  void (* clrchar)  (int,int,int);            // (id,row,col);
  void (* clrblk)   (int,int,int,int);        // (id,row,scol,ecol);
  void (* clrline)  (int,int);                // (id,row);
  void (* clrwin)   (int);

  /* draw */
  void (* drawchar) (char,int,int,int);       // (chr,id,row,col);
  void (* drawblk)  (char *,int,int,int,int); // (str,id,row,scol,ecol);
  void (* drawtext) (char *,int,int,int);     // (str,id,row,col);
  void (* drawline) (char *,int,int);         // (str,id,row);
  void (* refresh)  (char **,int);            // (arr,id);
} e3dif_t;

//======================================================================
// API functions
//======================================================================

/* buffer management */

int e3_buf_add_dif(int buf, e3dif_t *dif);
int e3_buf_select(int b);
int e3_buf_clear(int buf);
int e3_buf_discard(int buf);
int e3_buf_new(int rows, int cols, e3dif_t *dif);
int e3_buf_replace_dif(int buf, e3dif_t *dif);
int e3_buf_remove_dif(int buf);
int e3_buf_set_dif_stat(int buf, int stat);
int e3_buf_set_readonly(int b, int stat);
int e3_buf_set_linesize(int buf, int cols);
int e3_buf_get_info
(
  int b, int *count, int *lncount, int *longest, int *maxline
);

int e3_add_dif(e3dif_t *dif);
int e3_clear(void);
int e3_discard(void);
int e3_replace_dif(e3dif_t *dif);
int e3_remove_dif(void);
int e3_set_dif_stat(int stat);
int e3_set_linesize(int cols);
int e3_get_info(int *count, int *lncount, int *longest, int *maxline);

/* text import & export */

int e3_buf_copy_arr(int buf, e3arr_t *dest);
int e3_buf_copy_str(int buf, e3str_t *dest);
int e3_discard_array_export(e3arr_t *txt);
int e3_discard_string_export(e3str_t *txt);
int e3_buf_export_file(int buf, char *name);
int e3_buf_arr_info(int buf, int *count, int *len);
int e3_buf_str_info(int buf, int *size);
int e3_buf_import_arr(int buf, e3arr_t *txt);
int e3_buf_import_file(int buf, char *name);
int e3_buf_import_str(int buf, e3str_t *txt);

e3str_t *e3_string_export_new(int size);
e3str_t *e3_buf_export_str(int buf);

e3arr_t *e3_array_export_new(int count, int len);
e3arr_t *e3_buf_export_arr(int buf);

int e3_copy_arr(e3arr_t *dest);
int e3_copy_str(e3str_t *dest);
int e3_export_file(char *name);
int e3_arr_info(int *count, int *len);
int e3_str_info(int *size);
int e3_import_arr(e3arr_t *txt);
int e3_import_file(char *name);
int e3_import_str(e3str_t *txt);

e3str_t *e3_export_str(void);
e3arr_t *e3_export_arr(void);

/* editing */
int e3_buf_eval_cmd(int b, int cmd, int key);
int e3_buf_eval_key(int b, int key);
int e3_buf_install_commands(int b, int tab);
int e3_buf_refresh(int b);
int e3_buf_set_prefix_stat(int b, int stat);

int e3_add_command_table(e3cmd_t defs[]);
int e3_define_command(int tab, int cmd, int key, int prefix);
int e3_eval_cmd(int cmd, int key);
int e3_eval_key(int key);
int e3_init(void);
int e3_install_commands(int tab);
int e3_refresh(void);
int e3_set_prefix_stat(int stat);

/* wrapping */
int e3_buf_reflow_text(int b, int width);
int e3_buf_strip_space(int b, int loc);
int e3_buf_set_wrap(int b, int stat);

int e3_reflow_text(int width);
int e3_strip_space(int loc);
int e3_set_wrap(int stat);

/* error handling */
int   e3_last_err(void);
char *e3_errmsg(int errnum);

/* cursor */
int e3_buf_set_cursor_stat(int b, int stat);
int e3_set_cursor_stat(int stat);

/* keyboard */
int e3_set_keyboard_table(char normal[], char shifted[]);

/* debug */
void e3_log(char *fmt, ...);
void e3_setlog(int stat);
void e3_trace_off(void);
void e3_trace_on(void);

#endif  /* E3_H */

