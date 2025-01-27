int N = 8;
int board[8];

int is_safe(int row, int col) {
    int i = 0;

    for (i = 0; i < row; i++) {
        if (board[i] == col)
            return 0;
        if (board[i] - i == col - row)
            return 0;
        if (board[i] + i == col + row)
            return 0;
    }
    return 1;
}

int solve(int row) {
    int i = 0;
    int j = 0;

    if (row == N) {
        for (i = 0; i < N; i++) {
            for (j = 0; j < N; j++) {
                if (board[i] == j) {
                    printf("Q ");
                }
                else {
                    printf(". ");
                }
            }
            printf("\n");
        }
        printf("\n");
        return 1;
    }

    int count = 0;
    int col = 0;
    for (col = 0; col < N; col++) {
        if (is_safe(row, col)) {
            board[row] = col;
            count += solve(row + 1);
        }
    }

    return count;
}

int main() {
    int i = 0;
    int count = 0;

    for (i = 0; i < N; i++)
        board[i] = -1;

    count = solve(0);
    printf("Solution count = % d\n", count);

    return 0;
}