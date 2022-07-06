import { expect } from "chai";
import { ethers } from "hardhat";

import { MostValuableToken } from "../typechain"

const TOKEN_SUPPLY = 1000000;

describe("MostValuableToken", function () {

    let mvt: MostValuableToken;
    
    beforeEach(async () => {
        const MostValuableToken = await ethers.getContractFactory("MostValuableToken");
        mvt = await MostValuableToken.deploy(TOKEN_SUPPLY);
        await mvt.deployed();
    })
  it("Should return the proper token name and symbol", async function () {

    expect(await mvt.symbol()).to.equal("MVT");
  });
});