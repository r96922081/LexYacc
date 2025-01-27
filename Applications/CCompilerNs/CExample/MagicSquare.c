int magicSquare[15][15];

void generateMagicSquare(int n) {
    int i;
    int j;
    int new_i;
    int new_j;

    int num = 1;
    i = 0;
    j = n / 2;

    while (num <= n * n) {
        magicSquare[i][j] = num;
        num++;

        new_i = i - 1;
        new_j = j + 1;

        if (new_i < 0) {
            new_i = n - 1;
        }
        if (new_j >= n) {
            new_j = 0;
        }

        if (magicSquare[new_i][new_j] != 0) {
            new_i = i + 1;
            if (new_i >= n) 
                new_i = 0;
            new_j = j;
        }

        i = new_i;
        j = new_j;
    }

    printf("Magic Square of size %d:\n", n);
    for (i = 0; i < n; i++) {
        for (j = 0; j < n; j++) {
            printf("%4d", magicSquare[i][j]);
        }
        printf("\n");
    }
}

int main() {
    generateMagicSquare(7);
    return 0;
}