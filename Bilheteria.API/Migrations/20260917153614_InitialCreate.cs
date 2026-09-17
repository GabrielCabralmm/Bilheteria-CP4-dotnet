using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bilheteria.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tb_filme",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    c_titulo = table.Column<string>(type: "NVARCHAR2(150)", maxLength: 150, nullable: false),
                    c_genero = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: true),
                    c_duracao_minutos = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    c_classificacao_indicativa = table.Column<string>(type: "NVARCHAR2(10)", maxLength: 10, nullable: true),
                    c_sinopse = table.Column<string>(type: "NVARCHAR2(2000)", maxLength: 2000, nullable: true),
                    c_em_cartaz = table.Column<bool>(type: "NUMBER(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_filme", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tb_produto",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    c_nome = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    c_categoria = table.Column<string>(type: "NVARCHAR2(20)", maxLength: 20, nullable: false),
                    c_preco = table.Column<decimal>(type: "NUMBER(10,2)", nullable: false),
                    c_quantidade_estoque = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_produto", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tb_sessao",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    c_filme_id = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    c_data_hora_sessao = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    c_sala = table.Column<string>(type: "NVARCHAR2(20)", maxLength: 20, nullable: false),
                    c_preco_ingresso = table.Column<decimal>(type: "NUMBER(10,2)", nullable: false),
                    c_capacidade_total = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_sessao", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tb_sessao_tb_filme_c_filme_id",
                        column: x => x.c_filme_id,
                        principalTable: "tb_filme",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tb_pedido",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    c_sessao_id = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    c_nome_cliente = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    c_data_hora_pedido = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    c_valor_total = table.Column<decimal>(type: "NUMBER(10,2)", nullable: false),
                    c_status_pedido = table.Column<string>(type: "NVARCHAR2(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_pedido", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tb_pedido_tb_sessao_c_sessao_id",
                        column: x => x.c_sessao_id,
                        principalTable: "tb_sessao",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tb_ingresso",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    c_pedido_id = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    c_sessao_id = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    c_fileira = table.Column<string>(type: "NVARCHAR2(2)", maxLength: 2, nullable: false),
                    c_numero_assento = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    c_preco_pago = table.Column<decimal>(type: "NUMBER(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_ingresso", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tb_ingresso_tb_pedido_c_pedido_id",
                        column: x => x.c_pedido_id,
                        principalTable: "tb_pedido",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tb_ingresso_tb_sessao_c_sessao_id",
                        column: x => x.c_sessao_id,
                        principalTable: "tb_sessao",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tb_item_pedido",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    c_pedido_id = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    c_produto_id = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    c_quantidade = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    c_preco_unitario = table.Column<decimal>(type: "NUMBER(10,2)", nullable: false),
                    c_subtotal = table.Column<decimal>(type: "NUMBER(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_item_pedido", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tb_item_pedido_tb_pedido_c_pedido_id",
                        column: x => x.c_pedido_id,
                        principalTable: "tb_pedido",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tb_item_pedido_tb_produto_c_produto_id",
                        column: x => x.c_produto_id,
                        principalTable: "tb_produto",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IDX_ASSENTO_SESSAO",
                table: "tb_ingresso",
                columns: new[] { "c_sessao_id", "c_fileira", "c_numero_assento" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tb_ingresso_c_pedido_id",
                table: "tb_ingresso",
                column: "c_pedido_id");

            migrationBuilder.CreateIndex(
                name: "IX_tb_item_pedido_c_pedido_id_c_produto_id",
                table: "tb_item_pedido",
                columns: new[] { "c_pedido_id", "c_produto_id" });

            migrationBuilder.CreateIndex(
                name: "IX_tb_item_pedido_c_produto_id",
                table: "tb_item_pedido",
                column: "c_produto_id");

            migrationBuilder.CreateIndex(
                name: "IX_tb_pedido_c_data_hora_pedido",
                table: "tb_pedido",
                column: "c_data_hora_pedido");

            migrationBuilder.CreateIndex(
                name: "IX_tb_pedido_c_sessao_id",
                table: "tb_pedido",
                column: "c_sessao_id");

            migrationBuilder.CreateIndex(
                name: "IX_tb_produto_c_categoria",
                table: "tb_produto",
                column: "c_categoria");

            migrationBuilder.CreateIndex(
                name: "IX_tb_sessao_c_filme_id_c_data_hora_sessao",
                table: "tb_sessao",
                columns: new[] { "c_filme_id", "c_data_hora_sessao" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tb_ingresso");

            migrationBuilder.DropTable(
                name: "tb_item_pedido");

            migrationBuilder.DropTable(
                name: "tb_pedido");

            migrationBuilder.DropTable(
                name: "tb_produto");

            migrationBuilder.DropTable(
                name: "tb_sessao");

            migrationBuilder.DropTable(
                name: "tb_filme");
        }
    }
}
