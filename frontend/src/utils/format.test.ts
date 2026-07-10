import { describe, expect, it } from 'vitest';
import { money } from './format';

describe('money', () => {
  it('formats values in Brazilian reais', () => {
    expect(money(1234.5)).toMatch(/R\$\s?1\.234,50/);
  });

  it('preserves negative balances', () => {
    expect(money(-25)).toContain('-');
  });
});
