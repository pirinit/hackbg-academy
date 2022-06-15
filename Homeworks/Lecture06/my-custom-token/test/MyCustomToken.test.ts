import { expect } from "chai";
import { ethers } from "hardhat";
import { MyCustomToken } from "../typechain";
import { SignerWithAddress } from "@nomiclabs/hardhat-ethers/signers";

describe("MyCustomToken", function () {
  let testToken: MyCustomToken;
  let admin: SignerWithAddress;
  let user: SignerWithAddress;

  beforeEach(async function () {
    [admin, user] = await ethers.getSigners();
    const MyCustomToken = await ethers.getContractFactory("MyCustomToken");
    testToken = await MyCustomToken.deploy(ethers.utils.parseEther("100"));
  });

  it("should have correct initial supply", async function () {
    expect(await testToken.totalSupply()).to.eq(ethers.utils.parseEther("100"));
  });

  it("should allow owner to mint new tokens", async function () {
    await expect(testToken.connect(admin).mint(ethers.utils.parseEther("1")))
      .to.emit(testToken, "Transfer")
      .withArgs(
        ethers.constants.AddressZero,
        admin.address,
        ethers.utils.parseEther("1")
      );
  });

  it("should not allow regular user to mint", async function () {
    await expect(
      testToken.connect(user).mint(ethers.utils.parseEther("1"))
    ).to.be.revertedWith("Ownable: caller is not the owner");
  });
  
  it("should allow owner to mint after 24 hours have passed from the last mint operation", async function () {
    // first mint operation should succeed
    await expect(testToken.connect(admin).mint(ethers.utils.parseEther("1")))
      .to.emit(testToken, "Transfer");

    const oneDayInSeconds = 24 * 60 * 60;
    await ethers.provider.send('evm_increaseTime', [oneDayInSeconds+1]);

    await expect(
      testToken.connect(admin).mint(ethers.utils.parseEther("1"))
    ).to.emit(testToken, "Transfer")
    .withArgs(
      ethers.constants.AddressZero,
      admin.address,
      ethers.utils.parseEther("1")
    );
  });
  
  it("should not allow owner to mint before 24 hours have passed from the last mint operation", async function () {
    // first mint operation should succeed
    await expect(testToken.connect(admin).mint(ethers.utils.parseEther("1")))
      .to.emit(testToken, "Transfer");

    const oneDayInSeconds = 24 * 60 * 60;
    await ethers.provider.send('evm_increaseTime', [oneDayInSeconds-1]);

    await expect(
      testToken.connect(admin).mint(ethers.utils.parseEther("1"))
    ).to.be.revertedWith("MintNotAllowed");
  });
  
});
